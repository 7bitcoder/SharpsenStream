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

  send(textinput: HTMLTextAreaElement) {
    debugger;
    if (textinput.value) {
      this.chat$.sendData({message: textinput.value, userName: 'me', color:1});
      textinput.value = "";
    }
  }

  initializeChat(chatId: number) {
    if(!chatId)
      return;
    this.chat$.connect();
    const msg: Message = {
      message: `${chatId.toString()};1`,
      userName: 'me',
      color: 1
    }
    this.chat$.sendData(msg);
    this.chat$.getObservable().subscribe(
      {
        next: message => this.updateChat(message)
      }
    )
  }

  updateChat(message: Message) {
    message.color = this.toColor(message.color);
    this.messages.push(message);
  }
 
  toColor(num) {
      num >>>= 0;
      var b = num & 0xFF,
          g = (num & 0xFF00) >>> 8,
          r = (num & 0xFF0000) >>> 16,
          a = ( (num & 0xFF000000) >>> 24 ) / 255 ;
      return "rgba(" + [r, g, b, a].join(",") + ")";
  }
}
