import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomeComponent } from './content/home/home.component';
import { NavbarComponent } from './navbar/navbar.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatSidenavModule } from '@angular/material/sidenav'
import { MatToolbarModule } from '@angular/material/toolbar'
import { MatListModule } from '@angular/material/list'
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatMenuModule } from '@angular/material/menu';
import { SignInDialogComponent } from './navbar/sign-in-dialog/sign-in-dialog.component';
import { SidenavComponent } from './sidenav/sidenav.component';
import { ChatComponent } from './content/stream/chat/chat.component';
import { ContentComponent } from './content/content.component';
import { StreamComponent } from './content/stream/stream.component';
import { PageNotFoundComponent } from './content/page-not-found/page-not-found.component';
import { HttpClientModule } from '@angular/common/http';
import { VideoPlayerComponent } from './content/stream/video-player/video-player.component';
import { API_BASE_URL } from './api/Api';
import { StreamInfoComponent } from './content/stream/stream-info/stream-info.component';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { TokenInterceptor } from './shared/token-inceptor';

export function getBaseUrl(): string {
  return 'https://localhost:5001';
}
@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    NavbarComponent,
    SignInDialogComponent,
    SidenavComponent,
    ChatComponent,
    ContentComponent,
    StreamComponent,
    PageNotFoundComponent,
    VideoPlayerComponent,
    StreamInfoComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    MatToolbarModule,
    MatSidenavModule,
    MatListModule,
    MatMenuModule,
    MatButtonModule,
    MatIconModule,
    MatDialogModule,
    MatFormFieldModule,
    HttpClientModule
  ],
  providers: [
    { provide: API_BASE_URL, useFactory: getBaseUrl },
    HttpClientModule,
    { provide: HTTP_INTERCEPTORS, useClass: TokenInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
