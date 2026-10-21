# src/Hwpx2Hwp.HwpCom/
역할: 한/글 COM 사용법(API·스레드·오류 해석)이나 보안 모듈 등록 방식이 바뀔 때 수정된다
공개 진입점: HwpComEngine.Create(exeDir) → IConversionEngine — 외부는 이것만 호출
의존: 들어오는 것 App/ · 나가는 것 Core/(IConversionEngine·Model 만) · 금지: Core 흐름(BatchRunner 등) 호출, WinForms
| 파일 | 역할 |
|---|---|
| HwpComEngine.cs | IConversionEngine 구현·시작(설치 확인·Notices)·COM 예외 → FailureReason·서버 사망 시 1회 재생성 |
| StaDispatcher.cs | STA 전용 스레드 1개 + 작업 큐, 결과를 Task 로 돌려줌 |
| HwpSession.cs | dynamic 한/글 인스턴스(생성·숨김·RegisterModule·Open·SaveAs·Clear·Quit) — dynamic 은 이 파일에만 |
| HwpFormatMap.cs | 확장자 → 한/글 형식 문자열("HWPX"·"HWP") (V3) |
| SecurityModule.cs | F6 exe 폴더 DLL 찾기·HKCU 레지스트리 값 1개 쓰기 |
테스트: 맥 자동 테스트 없음. Windows 수동 T1.1·T1.2·T1.3·T6.1 (docs/test/windows-manual.md)
규칙: 한/글 2014 에 있는 COM API 만 쓴다. ProgID HWPFrame.HwpObject, late binding(interop 어셈블리 금지).
규칙: 모든 COM 호출은 StaDispatcher 스레드에서. 인스턴스는 앱 수명 동안 1개 재사용, 파일마다 Clear(1) 로 닫는다.
규칙: 레지스트리는 HKCU\Software\HNC\HwpAutomation\Modules 의 값 FilePathCheckerModule 하나만 쓴다(같으면 생략, 삭제 없음, HKLM 금지).
규칙: Open 인자에 suspendpassword:true 를 넣어 암호 대화상자를 막는다. 파일 단위 실패는 예외 대신 ConversionOutcome.
ADR: docs/design/baseline.md (D-6, D-7)
