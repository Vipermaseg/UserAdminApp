import { Component, Inject, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatDialogModule, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';

@Component({
  selector: 'result-modal',
  standalone: true,
  imports: [CommonModule, MatIconModule, MatDialogModule, MatProgressSpinnerModule, MatButtonModule, MatInputModule],
  templateUrl: './result-modal.component.html',
  styleUrls: ['./result-modal.component.scss']
})
export class ResultModalComponent {
  constructor(
    public dialogRef: MatDialogRef<ResultModalComponent>,
    @Inject(MAT_DIALOG_DATA) public data: {
      icon: string;
      title: string;
      subtitle: string;
      description: string;
      closeButtonText: string;
      onClose: Function;
      loading: boolean;
      textbox: {
        label: string;
        text: string
      };
      postdata: string;
      isError: boolean;
      disableClose: boolean;
    }
  ) {
    if (data.isError) {
      this.data.isError = data.isError;
    } else {
      this.data.isError = false; // Por defecto no es error
    }
    if (data.disableClose) {
      this.dialogRef.disableClose = data.disableClose;
    } else {
      this.dialogRef.disableClose = true; // Así no se puede salir de los modales clickando fuera a menos que sea explícito.
    }
}

  onClose(): void {
    this.data.onClose();
    this.dialogRef.close();
  }
}
