import { NgModule } from '@angular/core';
import { Routes, RouterModule, RoutesRecognized } from '@angular/router';
import { HomeComponent } from './content/home/home.component';
import { PageNotFoundComponent } from './content/page-not-found/page-not-found.component';
import { StreamComponent } from './content/stream/stream.component';
import { RouteNames } from './route-names';


const routes: Routes = [
  { path: '', component: HomeComponent, pathMatch: 'full' },
  { path: RouteNames.notFound, component: PageNotFoundComponent },
  { path: '**', component: StreamComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
