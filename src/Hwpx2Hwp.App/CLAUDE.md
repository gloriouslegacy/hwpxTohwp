# src/Hwpx2Hwp.App/
역할: 화면 구성·사용자 조작·결과 표시가 바뀔 때, 또는 엔진 조립이 바뀔 때 수정된다
공개 진입점: Program.Main (WinExe hwpx2hwp.exe) — 다른 프로젝트가 참조하지 않는다
의존: 들어오는 것 없음 · 나가는 것 Core/, HwpCom/(HwpComEngine.Create 만) · 금지: dynamic·ProgID·레지스트리, 변환 규칙(경로 계산·중복 제거 등) 직접 구현
| 파일 | 역할 |
|---|---|
| Program.cs | 조립 지점(엔진·BatchRunner·로그 폴더)·전역 예외 처리·종료 시 엔진 Dispose |
| MainForm.cs | 끌어다 놓기·[추가]·[변환]·[취소]·[로그 열기] 이벤트를 Core 에 연결, 진행 이벤트 표시 |
| MainFormLayout.cs | 컨트롤 생성·배치(코드로만) |
테스트: 맥 자동 테스트 없음. dotnet build Hwpx2Hwp.sln 로 빌드 확인, Windows 수동 T4.3
배포: dotnet publish src/Hwpx2Hwp.App/Hwpx2Hwp.App.csproj -c Release -r win-x64 -o artifacts/publish
규칙: SelfContained·단일 파일 설정은 RuntimeIdentifier 가 있을 때만 적용한다(맥 빌드 NETSDK1082 회피). 조건을 지우지 않는다.
규칙: 디자이너 파일(*.Designer.cs·.resx) 쓰지 않는다. 컨트롤은 MainFormLayout.cs 에서 만든다.
규칙: 수집·변환은 UI 스레드에서 돌리지 않는다(Task.Run / BatchRunner.RunAsync). 진행은 Progress<T> 로 받는다.
규칙: 화면 상태는 Core 이벤트의 사본이다. 작업 상태를 화면에서 따로 계산하지 않는다.
규칙: PublishTrimmed·ReadyToRun 켜지 않는다. 실패 이유 문구는 FailureReasonText 를 쓴다.
ADR: docs/design/baseline.md (D-8, D-9)
