import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

interface ChannelData {
  channelName: string;
  data: string;
}

@Injectable({
  providedIn: 'root'
})
export class StateService {

  constructor() { }

  private channelDataSubject = new BehaviorSubject<ChannelData>({ channelName: '', data: '' });
  channelData$ = this.channelDataSubject.asObservable();

  updateChannelData(channelName: string, data: string) {
    this.channelDataSubject.next({ channelName, data });
  }
}