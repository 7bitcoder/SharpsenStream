import { HttpClient } from '@angular/common/http';
import { webSocket, WebSocketSubject } from 'rxjs/webSocket';
import { Injectable } from '@angular/core';
import { Message } from './message';
import { ServerConfig } from './server-config';

@Injectable({
  providedIn: 'root'
})
export class ChatService {
  StreamUrl = '/api/Stream';
  private socket$: WebSocketSubject<Message>;
   
  public connect(): void {
 
    if (!this.socket$ || this.socket$.closed) {
      this.socket$ = this.getNewWebSocket();
    }
  }

  getObservable(){
    return this.socket$.asObservable();
  }
 
  private getNewWebSocket() {
    console.log(ServerConfig.getUrl())
    return webSocket<Message>(ServerConfig.getUrl());
  }

  sendData(data: Message) {
    debugger;
    this.socket$.next( data );
  }

  close() {
    this.socket$.complete(); 
  }
}
