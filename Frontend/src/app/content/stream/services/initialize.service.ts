import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class InitializeService {
  StreamServerUrl = 'http://localhost:8000/live';

  private readonly chat = new Subject<number>();
  private readonly viewer = new Subject<string>();
  constructor() { }

  initializeChat(chatId: number) {
    this.chat.next(chatId);
  }

  initializeViewer(streamName: string) {
    this.viewer.next(`${this.StreamServerUrl}/${streamName}.flv`);
  }

  getChatObservable(): Observable<number> {
    return this.chat.asObservable();
  }

  getViewerObservable(): Observable<string> {
    return this.viewer.asObservable();
  }
}
