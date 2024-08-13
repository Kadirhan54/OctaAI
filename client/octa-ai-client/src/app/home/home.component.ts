import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ApiService } from '../api.service';
import { tap, catchError } from 'rxjs/operators';
import { of } from 'rxjs';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements OnInit {
  
  channels: string[] = [];

  constructor(public apiService: ApiService) {
    const cent = this.apiService.getCentrifuge();
    console.log(cent);
  }

  ngOnInit(): void {

    this.apiService.getChannels().subscribe(channels => {
      this.channels = channels;
    });
  }


}
