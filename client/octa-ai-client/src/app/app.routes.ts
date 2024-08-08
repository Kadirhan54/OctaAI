import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component'; // Import the HomeComponent
import { AiPromptComponent } from './ai-prompt/ai-prompt.component';

export const routes: Routes = [
  { path: '', component: HomeComponent }, // Default route to HomeComponent
  { path: 'prompt', component: AiPromptComponent }, // Route to AiPromptComponent
];
