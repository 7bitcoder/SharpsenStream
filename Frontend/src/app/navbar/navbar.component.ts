import { Component, OnInit } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { UserService } from 'src/app/shared/user.service';
import { User } from '../api/Api';
import { SignInDialogComponent } from './sign-in-dialog/sign-in-dialog.component';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent implements OnInit {
  loggedIn = false;
  user: User;
  constructor(
    public dialog: MatDialog, 
    private user$: UserService) {}


  ngOnInit(): void {
    this.user$.tryFetchUser().subscribe( succes => this.successLogin(succes))
  }
  
  successLogin(succes: boolean): void {
    if( succes ) {
      this.user = this.user$.getUser();
      this.loggedIn = true;
    }
  }

  showLogInDialog() {
    const dialogConfig = new MatDialogConfig();

    dialogConfig.disableClose = true;
    dialogConfig.autoFocus = true;
    dialogConfig.data = {}
    dialogConfig.minWidth = 400;
    
    const dialogRef = this.dialog.open(SignInDialogComponent, dialogConfig);

    
    dialogRef.afterClosed().subscribe(
      data => this.analyzeData(data)
  );
  }

  analyzeData(data: User) {
    debugger;
    if(!data) {
      this.loggedIn = false;
    } else {
      this.user = data;
      this.loggedIn = true;
    }
  }
}
