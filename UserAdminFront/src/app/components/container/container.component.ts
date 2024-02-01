import { ChangeDetectionStrategy, ChangeDetectorRef, Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NavigationEnd, NavigationStart, Router, RouterLink, RouterOutlet } from '@angular/router';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { Subscription } from 'rxjs';


@Component({
  selector: 'app-container',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './container.component.html',
  styleUrls: ['./container.component.scss'],
  imports: [CommonModule,
    MatToolbarModule,
    MatButtonModule,
    RouterOutlet,
    RouterLink,
    MatSidenavModule,
    MatProgressBarModule]
})
export class ContainerComponent {
  subscriptions: Subscription = new Subscription;
  readonly loadingRoute = signal(false);

  constructor(private router: Router) { }

  ngOnInit(): void {
    this.subscriptions.add(
      this.router.events.subscribe(event => {
        if (event instanceof NavigationStart) {
          this.loadingRoute.set(true);
        } else if (event instanceof NavigationEnd) {
          this.loadingRoute.set(false);
        }
      }));
  }

  ngOnDestroy(): void {
    this.subscriptions.unsubscribe()
  }
}


