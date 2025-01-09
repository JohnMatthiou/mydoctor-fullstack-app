import { Component, inject } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { UserService } from '../../../shared/services/user.service';

@Component({
  selector: 'app-sidemenu',
  standalone: true,
  imports: [RouterLink, RouterLinkActive],
  templateUrl: './sidemenu.component.html',
  styleUrl: './sidemenu.component.css'
})
export class SidemenuComponent {
  userService = inject(UserService);
  loggedInUser = this.userService.user;
}
