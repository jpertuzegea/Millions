
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { LoginModel } from '../../../Models/LoginModel';
import { ResultModel } from '../../../Models/ResultModel';
import { AuthenticationService } from '../../../Services/Authentications/authentication.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  form: FormGroup;

  constructor(private formBuilder: FormBuilder,private  AuthenticationService: AuthenticationService 
  ) { }


  Login() {

    let Fields = this.GetFields();

    this.AuthenticationService.Login(Fields).subscribe(

      ResultModel => {
        let Resu = ResultModel as ResultModel;

        let LoginModel = Resu.Data as LoginModel;

        if (LoginModel.IsLogued) {
          let TokenFull = atob(LoginModel.Token.split('.')[1]);
          localStorage.setItem("Token", LoginModel.Token);
        } else {
          alert("Usuario NO Logueado");
        }

      }, error => {

        if (error.status == 401) {
          alert("No Autorizado");
        } else {
          alert(JSON.stringify(error));
        }

      }
    );
  }
   
  ngOnInit(): void {
    this.form = this.formBuilder.group(
      {
        UserName: '',
        Password: '',
        IsLogued: '',
        Token: ''
        // ExpirationToken: ''
      }
    );
  }

  GetFields() {

    let Field = new LoginModel();

    Field.UserName = this.form.get("UserName").value;
    Field.Password = this.form.get("Password").value;
    Field.IsLogued = false;
    Field.Token = this.form.get("Token").value;
    // Field.ExpirationToken = this.form.get("ExpirationToken").value;

    return Field;

  }
   
}
