import { AfterViewInit, Component, ElementRef, OnInit, ViewChild, ViewChildren } from '@angular/core';
import { ActivatedRoute, Router} from '@angular/router';
import { RouteNames } from 'src/app/route-names';
import { StreamService } from './services/stream.service';
import { StreamDto } from './StreamDto';
import { InitializeService } from './services/initialize.service';

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
    private stream$: StreamService,
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
    this.stream$.getStream(name).subscribe(
      {
        next : res => {
          this.streamInfo = res
          if (!res) {
            this.router.navigate([RouteNames.notFound]);
            return;
          }
          this.initializatior.initializeChat(this.streamInfo.chatId);
        }
      }
    );
  }

}
