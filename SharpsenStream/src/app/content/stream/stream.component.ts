import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router} from '@angular/router';
import { RouteNames } from 'src/app/route-names';
import { StreamService } from './stream.service';

@Component({
  selector: 'app-stream',
  templateUrl: './stream.component.html',
  styleUrls: ['./stream.component.scss']
})
export class StreamComponent implements OnInit {

  streamName: string;
  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private stream$: StreamService) { 
    const url = this.route.snapshot.url;
    if (url.length !== 1) {
      this.router.navigate([RouteNames.notFound]);
    }
    let streamName = url[0].toString();
    this.getStream(streamName);
    
  }
  async getStream(name: string) {
    this.stream$.getStream(name).subscribe(
      {
        next : res => console.log(res)
      }
    );
  }

  ngOnInit(): void {
  }

}
