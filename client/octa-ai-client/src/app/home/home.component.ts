import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ApiService } from '../api.service';
import { tap, catchError } from 'rxjs/operators';
import { of } from 'rxjs';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements OnInit {
  counter = 0;

  constructor(public apiService: ApiService) {}

  ngOnInit(): void {
    // this.getData();
  }

  // getData(): void {
  //   this.apiService.getData().pipe(
  //     tap(data => {
  //       console.log('Data:', data);
  //     }),
  //     catchError(error => {
  //       console.error('Error:', error);
  //       return of(null); // Return a fallback value or handle the error in another way
  //     })
  //   ).subscribe();
  // }

  // postData(): void {
  //   const data = { key: 'value' };
  //   this.apiService.sendPrompt(data).pipe(
  //     tap(response => {
  //       console.log('Response:', response);
  //     }),
  //     catchError(error => {
  //       console.error('Error:', error);
  //       return of(null); // Return a fallback value or handle the error in another way
  //     })
  //   ).subscribe();
  // };
  
  increment() {
    this.counter++;
  }

}
