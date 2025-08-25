import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from '../../Services/Authentications/authentication.service';

@Component({
  selector: 'app-work-space',
  templateUrl: './work-space.component.html',
  styleUrls: ['./work-space.component.css']
})
export class WorkSpaceComponent implements OnInit {

  constructor(private AuthenticationService: AuthenticationService) { }

  ngOnInit(): void {
  }

  LogOut() {
    this.AuthenticationService.LogOut();
  }

}
