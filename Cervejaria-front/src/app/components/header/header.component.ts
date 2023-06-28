import { TokenService } from './../../service/token/token.service';
import { Component } from '@angular/core';


@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent {
  constructor(private tokenService: TokenService){}
  sair(){

    this.tokenService.removeToken;
  }
}
