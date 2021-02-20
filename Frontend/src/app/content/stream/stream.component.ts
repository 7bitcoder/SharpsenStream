import { Component, ElementRef, OnInit, ViewChildren } from '@angular/core';
import { ActivatedRoute, Router} from '@angular/router';
import { RouteNames } from 'src/app/route-names';
import { StreamClient, StreamDto } from 'src/app/api/Api';
import { PageNotFoundComponent } from '../page-not-found/page-not-found.component';

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
    private stream$: StreamClient) { 
    const url = this.route.snapshot.url;
    if (url.length !== 1) {
      this.notFound();
    }
    let streamName = url[0].toString();
    this.getStream(streamName);
    
  }

  ngOnInit(){

  }
  
  async getStream(name: string) {
    this.stream$.getStreamInfo(name).subscribe(
      {
        next: res => {
          if (!res) {
            this.notFound();
            return;
          }
          this.streamInfo = res
        },
        error: error => this.notFound()
      }
    );
  }

  notFound() {
    this.router.navigate([RouteNames.notFound]);
  }

}
