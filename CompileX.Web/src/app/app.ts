import { Component, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

/* Material */
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatCardModule } from '@angular/material/card';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { CodeRunnerService } from './app.service';


@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,

    MatToolbarModule,
    MatButtonModule,
    MatSelectModule,
    MatFormFieldModule,
    MatInputModule,
    MatCardModule,
    MatProgressBarModule
  ],
  templateUrl: './app.html',
  styleUrls: ['./app.css']
})
export class App {

  language = 'java';
  isRunning = false;

  codeMap: any = {
    java: `public class Main {
  public static void main(String[] args) {
    System.out.println("Hello Java");
  }
}`,

    python: `print("Hello Python")`
  };

  code = this.codeMap.java;

  output = '';
  error = '';

  constructor(
    private codeService: CodeRunnerService,
    private cdr: ChangeDetectorRef
  ) {}

  onLanguageChange() {
    this.code = this.codeMap[this.language];
  }

  run() {

    this.isRunning = true;
    this.output = '';
    this.error = '';

    this.codeService
      .runCode({
        code: this.code,
        language: this.language
      })
      .subscribe({

        next: (res) => {
          this.output = res.output;
          this.error = res.error;
          this.isRunning = false;
          this.cdr.detectChanges();
        },

        error: () => {
          this.error = 'Execution failed';
          this.isRunning = false;
          this.cdr.detectChanges();
        }

      });
  }
}
