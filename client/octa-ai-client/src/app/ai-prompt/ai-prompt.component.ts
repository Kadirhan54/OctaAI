import { Component } from '@angular/core';
import { ApiService } from '../api.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { catchError, map } from 'rxjs/operators';
import { HttpErrorResponse } from '@angular/common/http';
import { Observable, of, throwError } from 'rxjs';
import { v4 as uuidv4 } from 'uuid';
import { Centrifuge } from 'centrifuge';
import { CentrifugeService } from '../centrifugo.service';

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
  centrifuge: Centrifuge | null;
  channel: string = '';

  constructor(private apiService: ApiService, private centrifugeService: CentrifugeService) {
    this.centrifuge = this.centrifugeService.getCentrifuge();

    console.log('Centrifuge1:', this.centrifuge);
  }

  ngOnInit() {
    this.centrifuge = this.centrifugeService.getCentrifuge();
    console.log('Centrifuge2:', this.centrifuge);

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

  onSubmit(): void {
    if (this.promptText.trim()) {
      this.channel = uuidv4(); // Generate a new GUID

      this.subscribeToCentrifugo(this.channel);

      this.apiService.sendPrompt(this.promptText, this.channel).pipe(
        // map((res: any) => res as string),
        catchError(this.handleError) 
      ).subscribe({
        // next: () => {
        //   // this.response = JSON.stringify(res); // Convert the response to a string for display
        // },
        error: (err: any) => {
          console.error('An error occurred in component:', err);
        },
        complete: () => {
          console.log('Request complete');
        }
      });
  }}

  private subscribeToCentrifugo(channel:string): void {
      if (this.centrifuge) {
          // Subscribe to the Centrifugo channel
          const sub = this.centrifuge.newSubscription(channel);
  
          sub.on('publication', (ctx) => {
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

  private handleError(error: HttpErrorResponse): Observable<never> {
    console.error('An error occurred:', error.message);
    return throwError(() => new Error('Something bad happened; please try again later.'));
  }
  
}
