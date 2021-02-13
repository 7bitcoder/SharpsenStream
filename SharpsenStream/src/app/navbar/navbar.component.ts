import { Component, OnInit } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { SignInDialogComponent } from './sign-in-dialog/sign-in-dialog.component';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent implements OnInit {
  loggedIn = false;
  constructor(
    public dialog: MatDialog) {}


  ngOnInit(): void {
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

  analyzeData(data: any) {
    if(!data) {
      this.loggedIn = false;
      console.log("not logged");
    } else {
      this.loggedIn = true;
    }
  }
}
