import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthenticationService } from '../../Services/Authentications/authentication.service';

@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.css']
})
export class NavBarComponent implements OnInit {

  form: FormGroup;

  constructor(private formBuilder: FormBuilder, private Router: Router, private AuthenticationService: AuthenticationService) { }

  ngOnInit(): void {
  }
   
  LogOut() {
    this.AuthenticationService.LogOut();
  }
   
}
