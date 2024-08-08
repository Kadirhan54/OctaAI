import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private apiUrl = 'https://localhost:7227/api';
  public token: string | null = null;

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
        }),
        catchError(this.handleError)
      );
  }

  getToken(): string | null {
    console.log('getToken:', this.token);
    return this.token;
  }

  private handleError(error: any): Observable<never> {
    console.error('An error occurred:', error);
    return throwError(error);
  }
}