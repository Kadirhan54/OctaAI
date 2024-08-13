import { Component } from '@angular/core';
import { ApiService } from '../api.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { catchError, map } from 'rxjs/operators';
import { HttpErrorResponse } from '@angular/common/http';
import { Observable, of, Subscription, throwError } from 'rxjs';
import { v4 as uuidv4 } from 'uuid';
import { Centrifuge } from 'centrifuge';
import { StateService } from '../state.service';

@Component({
  selector: 'app-ai-prompt',
  standalone: true,
  imports: [FormsModule,CommonModule],
  templateUrl: './ai-prompt.component.html',
  styleUrl: './ai-prompt.component.css'
})
export class AiPromptComponent {
  promptText: string = '';
  response: string | null = null;
  centrifuge: Centrifuge | null = null;
  promptChannel: string = '';
  channels: string[] = [];
  channelData: { channelName: string, data: string } | null = null;
  channelDataMap: { [channelName: string]: string } = {};
  subscription: Subscription;

  constructor(private apiService: ApiService,private stateService: StateService) {
    this.subscription = new Subscription();
  }

  ngOnInit() {
    this.centrifuge = this.apiService.getCentrifuge();
    this.setupCentrifuge();

    this.apiService.getChannels().subscribe((channels: string[]) => {
      this.channels = channels;
      this.apiService.subscribeToChannels(this.channels);
    });

    this.subscription = this.stateService.channelData$.subscribe(data => {
      if (data.channelName) {
        console.log('Channel data received:', data);
        this.channelDataMap[data.channelName] = data.data;
      }
    });
  }

  ngOnDestroy() {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  onSubmit(): void {
    if (this.promptText.trim()) {
      this.promptChannel = uuidv4(); // Generate a new GUID

      console.log(this.centrifuge);

      this.subscribeToCentrifugo(this.promptChannel);

      this.apiService.sendPrompt(this.promptText, this.promptChannel).pipe(
        // map((res: any) => res as string),
        catchError(this.handleError) 
      ).subscribe({
        error: (err: any) => {
          console.error('An error occurred in component:', err);
        },
        complete: () => {
          console.log('Request complete');
        }
      });
  }}
  
  // for listening centrifugo
  setupCentrifuge() {
    if (this.centrifuge) {
      this.centrifuge.on('connecting', function (ctx) {
        console.log(`connecting: ${ctx.code}, ${ctx.reason}`);
      }).on('connected', function (ctx) {
        console.log(`connected over ${ctx.transport}`);
      }).on('disconnected', function (ctx) {
        console.log(`disconnected: ${ctx.code}, ${ctx.reason}`);
      }).connect();
    }
  }

  private handleError(error: HttpErrorResponse): Observable<never> {
    console.error('An error occurred:', error.message);
    return throwError(() => new Error('Something bad happened; please try again later.'));
  }

  // for listening channel
  private async subscribeToCentrifugo(channel:string): Promise<void> {
    if (this.centrifuge) {
        // Subscribe to the Centrifugo channel
        const sub = this.centrifuge.newSubscription(channel);

        await sub.on('publication', (ctx) => {
            document.title = ctx.data.value;
            this.response = ctx.data.value;
        }).on('subscribing', function (ctx) {
            console.log(`subscribing: ${ctx.code}, ${ctx.reason}`);
        }).on('subscribed', function (ctx) {
            console.log('subscribed', ctx);
        }).on('unsubscribed', function (ctx) {
            console.log(`unsubscribed: ${ctx.code}, ${ctx.reason}`);
        }).subscribe();
    }
  }
}
