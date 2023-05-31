import { CanActivateFn } from '@angular/router';

export const authUsuarioGuard: CanActivateFn = (route, state) => {
  return true;
};
