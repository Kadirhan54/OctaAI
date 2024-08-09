import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
import { Centrifuge } from 'centrifuge';
import { jwtDecode } from 'jwt-decode';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private apiUrl = 'https://localhost:7227/api';
  private token: string | null = null;
  private centrifuge: Centrifuge | null = null;
  private response :string = "";
  private channels: string[] = [];

  constructor(private http: HttpClient) {}

  sendPrompt(promptText: string, channel: string): Observable<any> {
    const formData = new FormData();
    formData.append('value', promptText);
    formData.append('channel', channel);

    return this.http.post<any>(`${this.apiUrl}/gemini/PromptText`, formData)
      .pipe(
        catchError(this.handleError)
      );
  }

  login(email: string, password: string): Observable<any> {
    const formData = new FormData();
    formData.append('Email', email);
    formData.append('Password', password);


    console.log('formData:', formData.get('Email'), formData.get('Password'));

    return this.http.post<any>(`${this.apiUrl}/Auth/login`, formData)
      .pipe(
        tap(response => {
          this.token = response.token; // Assuming the token is in the response object

          this.createCentrifuge();
        }),
        catchError(this.handleError)
      );
  }

  getToken(): string | null {
    console.log('getToken:', this.token);
    return this.token;
  }

  createCentrifuge(): Centrifuge | null {
    // TODO : create jwt token according to centrifuge. the token that created by backend doesnt subribe to channel
    // const token = this.apiService.getToken();
    // this.token= "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJrYWRpcmhhbiIsImV4cCI6MTcyMzgxNzMzMywiaWF0IjoxNzIzMjEyNTMzfQ.Rj1Q-snqXr34MQgdzz4acUhJIZ_Fw6CnSv1vscqhpJA";

    if (!this.token) {
      console.error('Token is null, cannot create Centrifuge instance.');
      return null;
    }

    try {
      const decodedToken: any = jwtDecode(this.token);
      const channel = decodedToken.channels;

      console.log('Decoded token:', decodedToken);
      console.log('Channel:', channel);

    } catch (error) {
      console.error('Failed to decode token:', error);
      return null;
    }
  
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
    console.log('Centrifuge created with channel:', this.channels);

    return this.centrifuge;

  }

  getCentrifuge(): Centrifuge | null {
    return this.centrifuge;
  }

  private handleError(error: any): Observable<never> {
    console.error('An error occurred:', error);
    return throwError(error);
  }
}