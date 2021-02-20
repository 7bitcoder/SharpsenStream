import { AfterViewInit, Component, DebugEventListener, ElementRef, Input, OnInit, QueryList, ViewChild, ViewChildren } from '@angular/core';
import { ChatService } from './chat.service';
import { Message } from './message';
import { MessageDisplay } from './message-display';
import { StreamDto } from 'src/app/api/Api';


@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.scss']
})
export class ChatComponent implements AfterViewInit {
  @Input('streamInfo') streamInfo: StreamDto
  @ViewChild('scrollframe', {static: false}) scrollFrame: ElementRef;
  @ViewChildren('messages') itemElements: QueryList<any>;
  
  private messagesId = 0;
  messages: MessageDisplay[] = Array.from({length: 550}, ():MessageDisplay => {
    return {
      id: this.messagesId++,
      color: 'white',
      message: "nygger" + this.messagesId,
      userName: "monke"
    }
  })  ;
  private isNearBottom = true;
  private scrollContainer: any;

  constructor(private chat$: ChatService) { }

  ngOnInit(): void {
    this.initializeChat(this.streamInfo.chatId);
  }

  send(textinput: HTMLTextAreaElement) {
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
    this.messages.push({
      color: this.toColor(message.color),
      id: this.messagesId++,
      message: message.message,
      userName: message.userName
    });
    if (this.messages.length > 500) {
      // remove first element
      this.messages.shift();
    }
  }
 
  toColor(num) {
      num >>>= 0;
      var b = num & 0xFF,
          g = (num & 0xFF00) >>> 8,
          r = (num & 0xFF0000) >>> 16,
          a = ( (num & 0xFF000000) >>> 24 ) / 255 ;
      return "rgba(" + [r, g, b, a].join(",") + ")";
  }


  ngAfterViewInit() {
    this.scrollContainer = this.scrollFrame.nativeElement;  
    this.itemElements.changes.subscribe(_ => this.onItemElementsChanged());    
  }
  
  private onItemElementsChanged(): void {
    if (this.isNearBottom) {
      this.scrollToBottom();
    }
  }

  private scrollToBottom(): void {
    this.scrollContainer.scroll({
      top: this.scrollContainer.scrollHeight,
      left: 0,
      behavior: 'smooth'
    });
  }

  scrolled(event: any): void {
    this.isNearBottom = this.isUserNearBottom();
  }
  
  private isUserNearBottom(): boolean {
    const threshold = 20;
    const position = this.scrollContainer.scrollTop + this.scrollContainer.offsetHeight;
    const height = this.scrollContainer.scrollHeight;
    return position > height - threshold;
  }

  trackByFn(index: number, item: MessageDisplay): number {
    return item.id;
  }
}
