import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { EsqueciSenhaComponent } from './pages/esqueci-senha/esqueci-senha.component';
import { ContentComponent } from './layouts/content/content.component';
import { FullComponent } from './layouts/full/full.component';
import { CadastroUsuarioComponent } from './pages/cadastro-usuario/cadastro-usuario.component';
import { LoginComponent } from './pages/login/login.component';
import { DashboardComponent } from './pages/dashboard/dashboard.component';
import { authUsuarioGuard } from './security/auth-usuario.guard';
import { ReceitaComponent } from './pages/receita/receita.component';
import { ModificaReceitaComponent } from './pages/receita/modifica-receita/modifica-receita.component';
import { IngredienteComponent } from './pages/ingrediente/ingrediente.component';
import { ModificaIngredienteComponent } from './pages/ingrediente/modifica-ingrediente/modifica-ingrediente.component';

const routes: Routes = [
  {
    path: '',
    component: ContentComponent,
    children:[
      {path: '',
      redirectTo: 'login',
      pathMatch: 'full'
      },
     {
        path: 'login',
        component:LoginComponent

      },
      {
      path: 'cadastro',
      component: CadastroUsuarioComponent
      },
      {
        path: 'recuperar',
        component: EsqueciSenhaComponent
      }
    ]
  },
  {
    path: '',
    component: FullComponent,
    canActivate:[authUsuarioGuard],
    children: [
      {
        path: 'home',
        component: DashboardComponent
      },
      {
        path: 'receitas',
        component: ReceitaComponent
      },
      {
        path: 'receitas/:id',
        component: ModificaReceitaComponent
      },
      {
      path: 'receitas/criar',
      component: ModificaReceitaComponent
      },
      {
        path:"ingredientes",
        component: IngredienteComponent
      },
      {
        path: 'ingredientes/:id',
        component: ModificaIngredienteComponent
      },
      {
      path: 'ingredientes/criar',
      component: ModificaIngredienteComponent
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
