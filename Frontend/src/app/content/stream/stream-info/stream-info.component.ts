import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { min } from 'rxjs/operators';
import { StreamDto } from 'src/app/api/Api';

@Component({
  selector: 'app-stream-info',
  templateUrl: './stream-info.component.html',
  styleUrls: ['./stream-info.component.scss']
})
export class StreamInfoComponent implements OnInit, OnDestroy {

  streamName: string
  streamTitle: string
  viewers: string
  uptime: string

  streamUptime: number; // in seconds
  @Input('streamInfo') streamInfo: StreamDto

  interval
  constructor() { }

  ngOnDestroy(): void {
    clearInterval(this.interval);
  }

  ngOnInit(): void {
    this.streamName = this.streamInfo.streamName;
    this.streamTitle = this.streamInfo.title;
    this.viewers = '1203';
    this.streamUptime = 20000;
    this.initTimeUpdater();
  }

  initTimeUpdater() {
    this.interval = setInterval(() => {
      this.streamUptime++;
      this.formatTime();
    },1000)
  }

  formatTime() {
    let seconds = this.streamUptime;
    let minutes = Math.floor(seconds / 60);
    let hours = Math.floor(minutes / 60);
    seconds = seconds % 60
    minutes = minutes % 60
    this.uptime = `${hours}:${minutes < 10 ? '0' : '' }${minutes}:${seconds < 10 ? '0' : '' }${seconds}`
  }
}
