import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor() { }

  isAuthenticated(): boolean {
    // Implement your logic to check if the user is authenticated
    return !!localStorage.getItem('token');  // Example: Check if token exists
  }

  // login(token: string) {
  //   localStorage.setItem('token', token);
  // }

  // logout() {
  //   localStorage.removeItem('token');
  // }
}
