import { Component } from '@angular/core';
import { IconServiceService } from './shared/icon-service.service';
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  constructor(private icon$: IconServiceService) {}
}
