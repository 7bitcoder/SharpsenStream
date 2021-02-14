import { Component, DebugEventListener, OnInit } from '@angular/core';
import { ChatService } from './chat.service';
import { InitializeService } from '../services/initialize.service';
import { Message } from './message';


@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.scss']
})
export class ChatComponent implements OnInit {

  messages: Message[] = []
  constructor(
    private chat$: ChatService,
    private initialozator: InitializeService) { }

  ngOnInit(): void {
    this.initialozator.getChatObservable().subscribe(
      {
        next: chatId =>  this.initializeChat(chatId)
      }
    );
  }

  send(message: string) {
    this.chat$.sendData({message: message, userName: 'me'});
  }

  initializeChat(chatId: number) {
    if(!chatId)
      return;
    this.chat$.connect();
    const msg: Message = {
      message: chatId.toString(),
      userName: 'me'
    }
    this.chat$.sendData(msg);
    this.chat$.getObservable().subscribe(
      {
        next: message => this.updateChat(message)
      }
    )
  }

  updateChat(message: Message) {
    this.messages.push(message);
  }
}
