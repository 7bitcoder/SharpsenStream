import { Component, ElementRef, OnInit, ViewChildren } from '@angular/core';
import { ActivatedRoute, Router} from '@angular/router';
import { RouteNames } from 'src/app/route-names';
import { InitializeService } from './services/initialize.service';
import { StreamClient, StreamDto } from 'src/app/api/Api';

@Component({
  selector: 'app-stream',
  templateUrl: './stream.component.html',
  styleUrls: ['./stream.component.scss']
})
export class StreamComponent implements OnInit {

  @ViewChildren('videoElement') videoElement: ElementRef;
  streamInfo: StreamDto;
  streamName: string;
  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private stream$: StreamClient,
    private initializatior: InitializeService) { 
    const url = this.route.snapshot.url;
    if (url.length !== 1) {
      this.router.navigate([RouteNames.notFound]);
    }
    let streamName = url[0].toString();
    this.getStream(streamName);
    
  }

  ngOnInit(){

  }
  
  async getStream(name: string) {
    this.stream$.getStreamInfo(name).subscribe(
      {
        next : res => {
          this.streamInfo = res
          if (!res) {
            this.router.navigate([RouteNames.notFound]);
            return;
          }
          this.initializatior.initializeChat(this.streamInfo.chatId);
          this.initializatior.initializeViewer(this.streamInfo.streamName);
        }
      }
    );
  }

}
