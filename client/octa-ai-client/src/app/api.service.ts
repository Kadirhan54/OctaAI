import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, of, throwError } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
import { Centrifuge } from 'centrifuge';
import { jwtDecode } from 'jwt-decode';
import { StateService } from './state.service';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private apiUrl = 'https://localhost:7227/api';
  private token: string | null = null;
  private decodedToken: string | null = null;
  private centrifuge: Centrifuge | null = null;
  private response :string = "";
  private channels: string[] = [];
  private userId :string = "";

  constructor(private http: HttpClient,private stateService: StateService) {}

  sendPrompt(promptText: string, channel: string): Observable<any> {
    const formData = new FormData();
    formData.append('value', promptText);
    formData.append('channel', channel);
    formData.append('UserId', this.userId);

    return this.http.post<any>(`${this.apiUrl}/gemini/PromptText`, formData)
    .pipe(
      catchError(this.handleError)
    );
  }

  // sendSubscription(channel: string): Observable<any> {
  //   const formData = new FormData();
  //   formData.append('Channel', channel);
  //   formData.append('UserId', this.userId);

  //   console.log('formData:', formData.get('Channel'), formData.get('UserId'));

  //   return this.http.post<any>(`${this.apiUrl}/Centrifugo/Subscribe`, formData)
  //     .pipe(
  //       catchError(this.handleError)
  //     );
  // }

  login(email: string, password: string): Observable<any> {
    const formData = new FormData();
    formData.append('Email', email);
    formData.append('Password', password);


    console.log('formData:', formData.get('Email'), formData.get('Password'));

    return this.http.post<any>(`${this.apiUrl}/Auth/login`, formData)
      .pipe(
        tap(response => {
          this.token = response.token; // Assuming the token is in the response object

          localStorage.setItem('token', this.token!);
          
          this.createCentrifuge();
        }),
        catchError(this.handleError)
      );
  }

  getToken(): string | null {
    console.log('getToken:', this.token);
    return this.token;
  }

  decodeToken(): void {
    try {
      const decodedToken: any = jwtDecode(this.token!);
      this.channels = JSON.parse(decodedToken.channels2);
      this.decodedToken = decodedToken;
      this.userId = decodedToken.sub;
    } catch (error) {
      console.error('Failed to decode token:', error);
    }
  }

  createCentrifuge(): Centrifuge | null {
  
    if (!this.token) {
      console.error('Token is null, cannot create Centrifuge instance.');
      return null;
    }

    this.decodeToken();
  
    // TODO update
    if (!this.channels) {
      console.error('Channel is not present in the token.');
      return null;
    }

    this.centrifuge = new Centrifuge("ws://localhost:8000/connection/websocket", {
      token: this.token,
    });
    
    console.log('Centrifuge created :', this.centrifuge);
    console.log('Centrifuge created with token:', this.token);
    
    this.subscribeToChannels(this.channels);

    console.log('Centrifuge created with channel:', this.channels);

    return this.centrifuge;
  }

  subscribeToChannels(channels: string[]): void {
    if (!this.centrifuge) {
      console.error('Centrifuge is null, cannot subscribe to channel.');
      return;
    }

    for (let i = 0; i < channels.length; i++) {
      const channel = channels[i];

      // console.log("i : ", i);
      // console.log("channel : ", channel);

      const sub = this.centrifuge.newSubscription(channel);

      sub.on('publication', (ctx) => {
          document.title = ctx.data.value;
          this.stateService.updateChannelData(channel, ctx.data.value);
      }).on('subscribing', function (ctx) {
          console.log(`subscribing: ${ctx.code}, ${ctx.reason}`);
      }).on('subscribed', function (ctx) {
          console.log('subscribed', ctx);
      }).on('unsubscribed', function (ctx) {
          console.log(`unsubscribed: ${ctx.code}, ${ctx.reason}`);
      }).subscribe();
    }
  }

  getCentrifuge(): Centrifuge | null {
    return this.centrifuge;
  }

  getChannels(): Observable<string[]> {
    if (!this.userId) {
      throw new Error('User ID is not set');
    }
    console.log('userId:', this.userId);
    const url = `${this.apiUrl}/centrifugo/channels?id=${encodeURIComponent(this.userId)}`;
    return this.http.get<string[]>(url);
  }

  getUserId(): string | null {
    return this.userId;
  }

  setUserId(userId: string): void {
    this.userId = userId;
  }

  private handleError(error: any): Observable<never> {
    console.error('An error occurred:', error);
    return throwError(error);
  }

  // async subscribeToChannel(channel: string): Promise<void> {
  //   if (!this.centrifuge) {
  //     console.error('Centrifuge is null, cannot subscribe to channel.');
  //     return;
  //   }
  
  //   const sub = this.centrifuge.newSubscription(channel);
  
  //   sub.on('publication', (ctx) => {
  //       document.title = ctx.data.value;
  //       this.response = ctx.data.value;
  //   }).on('subscribing', function (ctx) {
  //       console.log(`subscribing: ${ctx.code}, ${ctx.reason}`);
  //   }).on('subscribed', function (ctx) {
  //       console.log('subscribed', ctx);
  //   }).on('unsubscribed', function (ctx) {
  //       console.log(`unsubscribed: ${ctx.code}, ${ctx.reason}`);
  //   });
  
  //   await sub.subscribe();
  // }


}