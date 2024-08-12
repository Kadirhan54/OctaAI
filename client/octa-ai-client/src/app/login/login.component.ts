import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { ApiService } from '../api.service';
import { HttpErrorResponse } from '@angular/common/http';
import { catchError, Observable, throwError } from 'rxjs';
import { Centrifuge } from 'centrifuge';
@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ ReactiveFormsModule ,CommonModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  loginForm: FormGroup = new FormGroup({});
  centrifuge: Centrifuge | null = null;

  constructor(private apiService : ApiService  ,private fb: FormBuilder, private router: Router) {   }

  ngOnInit(): void {
    this.loginForm = this.fb.group({
      username: ['', Validators.required],
      password: ['', Validators.required]
    });
  }

  onSubmit(): void {
    if (this.loginForm.valid) {
      const username = this.loginForm.get('username')?.value;
      const password = this.loginForm.get('password')?.value;

      this.apiService.login(username, password).pipe(
        catchError(this.handleError) 
      ).subscribe({
        complete: () => {
          // On successful login, you can navigate to another page
          this.router.navigate(['/prompt']);
          console.log('Login complete');
        }
      });
    }
  }

  private handleError(error: HttpErrorResponse): Observable<never> {
    console.error('An error occurred:', error.message);
    return throwError(() => new Error('Something bad happened; please try again later.'));
  }
}
