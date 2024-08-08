import { Injectable } from '@angular/core';
import { Centrifuge } from 'centrifuge';
import { ApiService } from './api.service';

@Injectable({
  providedIn: 'root'
})
export class CentrifugeService {
  private centrifuge: Centrifuge | null = null;

  constructor(private apiService: ApiService) {
    this.initializeCentrifuge();
  }

  getCentrifuge(): Centrifuge | null {
    return this.centrifuge;
  }

  private initializeCentrifuge(): void {
    if (!this.centrifuge) {
      this.createCentrifuge();
    }
  }

  createCentrifuge(): Centrifuge | null {
    // TODO : create jwt token according to centrifuge. the token that created by backend doesnt subribe to channel
    // const token = this.apiService.getToken();
    const token= "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJleHAiOjE3MjM3NTQ5NzAsImlhdCI6MTcyMzE1MDE3MH0.sCEp0SX98IHdqI5IzzOAt4eaLkA3cE-gXuDtjDQ7X7g";

    if (!token) {
      console.error('Token is null, cannot create Centrifuge instance.');
      return null;
    }

    this.centrifuge = new Centrifuge("ws://localhost:8000/connection/websocket", {
      token: token
    });

    console.log('Centrifuge created:', this.centrifuge);
    return this.centrifuge;
  }
}