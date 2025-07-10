import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { authGuard } from './guards/auth.guard';
import { LayoutComponent } from './components/layout/layout.component';
import { UserComponent } from './components/user/user.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { QuestionComponent } from './components/question/question.component';
import { TopicComponent } from './components/topic/topic.component';
import { OptionComponent } from './components/option/option.component';

const routes: Routes = [
  {
    path: 'login',
    component: LoginComponent
  },
  {
    path: '',
    component: LayoutComponent,
    canActivate: [authGuard],
    children: [
      {
        path: 'dashboard',
        component: DashboardComponent
      },
      {
        path: 'users',
        component: UserComponent
      },
      {
        path: 'topics',
        component: TopicComponent
      },
      {
        path: 'questions',
        component: QuestionComponent
      },
      {
        path: 'options',
        component: OptionComponent
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
