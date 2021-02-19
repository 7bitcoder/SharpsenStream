import { Injectable } from '@angular/core';
import { MatIconRegistry } from '@angular/material/icon';
import { DomSanitizer } from '@angular/platform-browser';

@Injectable({
  providedIn: 'root'
})
export class IconServiceService {
  icons = {
      LogIn: '<svg style="width:24px;height:24px" viewBox="0 0 24 24"><path fill="currentColor" d="M10,17V14H3V10H10V7L15,12L10,17M10,2H19A2,2 0 0,1 21,4V20A2,2 0 0,1 19,22H10A2,2 0 0,1 8,20V18H10V20H19V4H10V6H8V4A2,2 0 0,1 10,2Z" /></svg>',
      SendMessage: '<svg style="width:24px;height:24px" viewBox="0 0 24 24"><path fill="currentColor" d="M4 6.03L11.5 9.25L4 8.25L4 6.03M11.5 14.75L4 17.97V15.75L11.5 14.75M2 3L2 10L17 12L2 14L2 21L23 12L2 3Z" /></svg>'
    }
  constructor(iconRegistry: MatIconRegistry, sanitizer: DomSanitizer) { 
    for (const property in this.icons) {
      iconRegistry.addSvgIconLiteral(property, sanitizer.bypassSecurityTrustHtml(this.icons[property]))
    }
  }
}
