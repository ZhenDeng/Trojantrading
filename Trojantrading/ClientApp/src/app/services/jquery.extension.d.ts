/// <reference types="jquery"/>

interface JQuery {
  showValidator(message: string, tp: string, position: string, isStayOut?: boolean, isStayAlive?: boolean, finishTime?: number,
    confirmCallback?: Function, confirmTheme?: string, confirmOptions?: string[]): void;
}