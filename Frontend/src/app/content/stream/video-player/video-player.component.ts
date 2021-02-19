import { AfterViewInit, Component, OnInit } from '@angular/core';
import FlvJs from 'flv.js';
import { InitializeService } from '../services/initialize.service';

@Component({
  selector: 'app-video-player',
  templateUrl: './video-player.component.html',
  styleUrls: ['./video-player.component.scss']
})
export class VideoPlayerComponent implements AfterViewInit {

  flvPlayer: FlvJs.Player
  constructor(private initializeService: InitializeService) { }

  ngOnInit(): void {
  }

  ngAfterViewInit(): void {
    this.initializeService.getViewerObservable().subscribe( streamUrl =>  this.initializeStream(streamUrl));
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
    debugger;
    try {
      if (FlvJs.isSupported()) {
        const videoElement = document.getElementById('videoElement') as HTMLMediaElement;
        this.flvPlayer = FlvJs.createPlayer({
          type: 'flv',
          "isLive": true,
          url: streamUrl
        });
        this.flvPlayer.attachMediaElement(videoElement);
        this.flvPlayer.load();
      }
    } finally{}
  }
}
