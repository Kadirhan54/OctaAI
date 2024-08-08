import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private apiUrl = 'https://localhost:7227/api/gemini';

  constructor(private http: HttpClient) {}

  sendPrompt(promptText: string, channel:string ): Observable<any> {

    const formData = new FormData();
    formData.append('value', promptText); 
    formData.append('channel', channel); // Add GUID to form data

    return this.http.post<any>(`${this.apiUrl}/PromptText`, formData)
      .pipe(
        catchError(this.handleError)
      );
  }

  getGeminiInfo(): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}`)
      .pipe(
        catchError(this.handleError)
      );
  }

  private handleError(error: HttpErrorResponse): Observable<never> {
    console.error('An error occurred in api service:', error.message);
    return throwError(() => new Error('Something bad happened; please try again later.'));
  }
}