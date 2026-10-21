# 세션 메모 (갱신 2026-10-21 · 커밋 없음, 미추적 상태)
## 끝낸 것 (최근)
- kickoff 완료: 요구사항·명세 F1~F6·baseline(확정)·structure·roadmap·모듈 CLAUDE.md 4장·docs/CLAUDE.md·docs/test/windows-manual.md
- 1단계 완료: sln·global.json(SDK 10.0.402)·Directory.Build.props·4 프로젝트·ArchitectureTests 3건
- 확인: 맥 `dotnet build Hwpx2Hwp.sln` 경고 0 / `dotnet test tests/Hwpx2Hwp.Core.Tests/Hwpx2Hwp.Core.Tests.csproj` 3/3 / 위반 주입 시 실패
- 배포: `dotnet publish src/Hwpx2Hwp.App/Hwpx2Hwp.App.csproj -c Release -r win-x64 -o artifacts/publish` → hwpx2hwp.exe 하나(pdb 없음, 약 52MB). Windows 실행은 미확인
## 열린 것
- 첫 git 커밋 안 함(사용자 요청 시). 레포 폴더를 옮길 예정 — 문서에 절대 경로 없음
- reviewer 낮음 3건: 아키텍처 테스트 주석 오탐·우회 일부·루트 Directory.Build.props 미검사(고칠지 미정)
- Windows 확인 대기: 보안 모듈 DLL 이름·비트, 2014 Open 인자, 암호 manifest, 260자 경로(baseline 열린 문제 1~4)
## 다음에
- 로드맵 2단계 Core 흐름. 첫 발주 2a: Model/·Engine/(IConversionEngine·EngineStart)·tests Fakes/ 가짜 엔진 (spec F1 T1.4)
- 이어서 2b InputCollector(F2) · 2c OutputPathPlanner(F3) · 2d HwpxProbe — 서로 파일이 겹치지 않으면 2a 뒤 병렬
- 발주마다 implementer → reviewer, lessons.md 관련 줄을 Note 로
