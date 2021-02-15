import { AfterViewInit, Component, OnInit } from '@angular/core';
import FlvJs from 'flv.js';

@Component({
  selector: 'app-video-player',
  templateUrl: './video-player.component.html',
  styleUrls: ['./video-player.component.scss']
})
export class VideoPlayerComponent implements AfterViewInit {

  flvPlayer: FlvJs.Player
  constructor() { }

  ngOnInit(): void {
  }

  ngAfterViewInit(): void {
    if (FlvJs.isSupported()) {
      const videoElement = document.getElementById('videoElement') as HTMLMediaElement;
      this.flvPlayer = FlvJs.createPlayer({
        type: 'flv',
        "isLive": true,
        url: 'http://localhost:8000/live/new.flv'
      });
      this.flvPlayer.attachMediaElement(videoElement);
      this.flvPlayer.load();
    }
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
}
