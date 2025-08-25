import { Component } from '@angular/core';
import { AuthenticationService } from './Services/Authentications/authentication.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {

  constructor(private AuthenticationService: AuthenticationService ) {
  }

  IsLogued(): boolean {
    return this.AuthenticationService.IsLoged(); 
  }


}
 
