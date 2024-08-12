import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../auth.service';

@Component({
  selector: 'app-nav',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './nav.component.html',
  styleUrl: './nav.component.css'
})
export class NavComponent {
  private loggedIn = false; // Simulate logged in state

  constructor(private router: Router, private authService : AuthService) {}

  isUserLoggedIn(): boolean {
    this.loggedIn = this.authService.isAuthenticated();
    
    return this.loggedIn;
  }

  logout(): void {
    this.loggedIn = false;
    
    localStorage.removeItem('token');

    this.router.navigate(['/login']); // Redirect to login page after logout
  }

  // Implement actual login/logout logic as needed
}
