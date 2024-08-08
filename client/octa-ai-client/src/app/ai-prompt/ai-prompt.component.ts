import { Component } from '@angular/core';
import { ApiService } from '../api.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { catchError, map } from 'rxjs/operators';
import { HttpErrorResponse } from '@angular/common/http';
import { Observable, of, throwError } from 'rxjs';

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

  constructor(private apiService: ApiService) {}

  onSubmit(): void {
    if (this.promptText.trim()) {
      this.apiService.sendPrompt(this.promptText).pipe(
        // map((res: any) => res as string),
        catchError(this.handleError)
      ).subscribe({
        next: (res: string) => {
          this.response = JSON.stringify(res); // Convert the response to a string for display
        },
        error: (err: any) => {
          console.error('An error occurred in component:', err);
        },
        // complete: () => {
        //   console.log('Request complete');
        // }

      });
  }}

  private handleError(error: HttpErrorResponse): Observable<never> {
    console.error('An error occurred:', error.message);
    return throwError(() => new Error('Something bad happened; please try again later.'));
  }
  
}
