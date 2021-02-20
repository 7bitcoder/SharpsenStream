import { AfterViewInit, Component, Input, OnInit } from '@angular/core';
import FlvJs from 'flv.js';
import { StreamDto } from 'src/app/api/Api';

@Component({
  selector: 'app-video-player',
  templateUrl: './video-player.component.html',
  styleUrls: ['./video-player.component.scss']
})
export class VideoPlayerComponent implements AfterViewInit {

  StreamServerUrl = 'http://localhost:8000/live';
  @Input('streamInfo') streamInfo: StreamDto

  flvPlayer: FlvJs.Player
  constructor() { }

  ngOnInit(): void {
  }

  ngAfterViewInit(): void {
    this.initializeStream(`${this.StreamServerUrl}/${this.streamInfo.streamName}.flv`)
  }

  play(): void {
    this.flvPlayer.play();
  }

  pause(): void {
    this.flvPlayer.pause();
  }

  showControlls() {

  }

  hideControlls() {

  }

  initializeStream(streamUrl: string) {
    try {
      if (FlvJs.isSupported()) {
        const videoElement = document.getElementById('videoElement') as HTMLMediaElement;
        this.flvPlayer = FlvJs.createPlayer({
          type: 'flv',
          url: streamUrl
        }, {
          enableStashBuffer: false // live
        });
        this.flvPlayer.attachMediaElement(videoElement);
        this.flvPlayer.load();
      }
    } finally{}
  }
}
