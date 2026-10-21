# 구조 색인 (확정 — docs/design/baseline.md)

의존 방향: App → HwpCom → Core, App → Core, tests → Core. Core 는 BCL 외 아무것도 참조하지 않는다.

| 모듈 | 역할 | 의존 |
|---|---|---|
| src/Hwpx2Hwp.Core | net10.0. 변환 흐름(수집 F2·경로 계산 F3·암호 사전 검사·일괄 실행 F4·로그 F5)·도메인 타입·엔진 인터페이스. 흐름 규칙이 바뀔 때 수정 | BCL 만 |
| src/Hwpx2Hwp.HwpCom | net10.0-windows. 한/글 2014 COM 엔진(F1)·STA 스레드·보안 모듈 등록(F6). 한/글 API 사용법이 바뀔 때 수정 | Core |
| src/Hwpx2Hwp.App | net10.0-windows WinForms. 화면·조립 지점(composition root). 화면이 바뀔 때 수정 | Core, HwpCom(`HwpComEngine.Create` 만) |
| tests/Hwpx2Hwp.Core.Tests | net10.0 xUnit. 맥에서 도는 자동 테스트(가짜 엔진)·아키텍처 테스트 | Core |

추가 예정(도입 조건): `src/Hwpx2Hwp.Cli`(V2, Core+HwpCom) · 직접 변환 엔진 프로젝트(V1, Core) · `tests/Hwpx2Hwp.HwpCom.Tests`(Windows 에서 dotnet test 가능해질 때).
