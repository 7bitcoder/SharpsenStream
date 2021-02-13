import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class InitializeService {

  private readonly chat = new Subject<number>();
  constructor() { }

  initializeChat(chatId: number) {
    this.chat.next(chatId);
  }

  getChatObservable(): Observable<number> {
    return this.chat.asObservable();
  }
}
