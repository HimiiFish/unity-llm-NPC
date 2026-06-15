# Unity LLM NPC Demo — 需求拆分与任务清单

> 文档版本：1.0  
> 适用对象：肖雨 · 游戏客户端 + AI 应用学习路线  
> 预计周期：12 周 · 约 20 小时/周  
> 最后更新：2026-06-15

---

## 一、项目概述

### 1.1 项目目标

在 Unity 中实现一个 **LLM 驱动的 NPC 对话 Demo（B 档）**，用于：

- 作品集展示（GitHub + 录屏 + README）
- 应届求职：**主投游戏客户端（Unity/C#）**，**备投游戏 AI / 智能化应用**
- 验证「大模型 + 传统游戏 AI（FSM/行为树）」的集成方案

### 1.2 已确认决策（Shared Understanding）

| 决策项 | 选择 |
|--------|------|
| 主方向 | 大模型 / 生成式 AI（对话 NPC、状态约束、结构化输出） |
| 辅方向 | 传统游戏 AI（FSM、行为树；GOAP/Utility 按需加深） |
| 学习节奏 | 混合：约 70% 项目 + 30% 理论 |
| 时间投入 | 约 20 小时/周 |
| Demo 功能边界 | **B 档**（见下文需求清单） |
| LLM 接入 | 国内云端 API，Unity 直连（第 7～10 周可演进到 Python 网关 + RAG） |
| 理论路线 | 按 Demo 里程碑补课，ML 系统学习整体延后 |

### 1.3 不在第一版范围内（Backlog）

- RAG 知识库检索（第二迭代）
- Python FastAPI 网关（第二迭代）
- 多 NPC 同时对话
- 语音输入/输出（TTS/STT）
- 本地 Ollama 模型
- HimiiEngine 集成（第三迭代 / 长期方向）
- 模型训练 / Unity ML-Agents

---

## 二、需求拆分

### 2.1 功能需求（B 档 MVP）

#### 模块 A：场景与交互

| 编号 | 需求 | 验收标准 | 优先级 |
|------|------|----------|--------|
| A-01 | 可控制玩家角色 | WASD 或点击移动，摄像机跟随 | P0 |
| A-02 | 至少 1 个 NPC | 场景内可见，可触发交互 | P0 |
| A-03 | 对话 UI | 输入框、发送按钮、NPC 回复展示区 | P0 |
| A-04 | 交互触发 | 玩家靠近 NPC 按键 / 点击打开对话 | P0 |
| A-05 | 对话状态 UI（可选） | 显示当前好感度、进行中任务 | P1 |

#### 模块 B：LLM 对话

| 编号 | 需求 | 验收标准 | 优先级 |
|------|------|----------|--------|
| B-01 | API 封装 | 支持发送消息并接收回复 | P0 |
| B-02 | System Prompt 人设 | NPC 性格、说话风格、世界观约束可配置 | P0 |
| B-03 | 多轮对话记忆 | 滑动窗口保留最近 N 轮历史 | P0 |
| B-04 | 结构化 JSON 输出 | LLM 返回固定 Schema，可解析为对象 | P0 |
| B-05 | 流式输出（可选） | 打字机效果展示回复 | P2 |
| B-06 | 请求中状态 | Loading 指示，防止重复发送 | P1 |

#### 模块 C：游戏状态联动

| 编号 | 需求 | 验收标准 | 优先级 |
|------|------|----------|--------|
| C-01 | GameState 数据 | 好感度、当前任务 ID、NPC 状态枚举 | P0 |
| C-02 | Prompt 注入状态 | 每次请求携带当前 GameState 摘要 | P0 |
| C-03 | JSON 驱动逻辑 | `affectionDelta` 更新好感度 | P0 |
| C-04 | 任务触发 | `triggerQuest` 非空时触发对应任务 | P0 |
| C-05 | 任务完成反馈 | 任务状态变化 reflected 到 UI 或 NPC 台词 | P1 |

#### 模块 D：FSM / 行为树约束

| 编号 | 需求 | 验收标准 | 优先级 |
|------|------|----------|--------|
| D-01 | NPC 状态机 | 至少：Idle / Busy / Combat / Talking | P0 |
| D-02 | 状态 gate | Combat / Busy 时拒绝对话或返回固定台词 | P0 |
| D-03 | 非对话行为（第 5 周） | Idle 时巡逻或待机动画 | P1 |
| D-04 | 状态切换可视化 | Inspector 或 Debug 面板可查看当前状态 | P2 |

#### 模块 E：健壮性与安全

| 编号 | 需求 | 验收标准 | 优先级 |
|------|------|----------|--------|
| E-01 | API Key 隔离 | 不进 Git、不进包体；使用本地 secrets 或环境变量 | P0 |
| E-02 | JSON 解析失败兜底 | 重试 1 次 + 固定 fallback 台词 | P0 |
| E-03 | 网络超时处理 | 超时提示，不卡死 UI | P0 |
| E-04 | 日志（Debug 模式） | 可开关的请求/响应日志 | P1 |

#### 模块 F：交付物

| 编号 | 需求 | 验收标准 | 优先级 |
|------|------|----------|--------|
| F-01 | GitHub 仓库 | 清晰目录、`.gitignore` 含 secrets | P0 |
| F-02 | README | 架构说明、运行步骤、Key 配置、录屏链接 | P0 |
| F-03 | 演示录屏 | 3～5 分钟，展示对话 + 好感度 + 任务 + 状态拒绝 | P0 |
| F-04 | 架构图 | README 内 Mermaid 或截图 | P1 |

### 2.2 非功能需求

| 编号 | 需求 | 说明 |
|------|------|------|
| NF-01 | 可维护性 | 分层清晰：UI / Brain / API / GameState / FSM |
| NF-02 | 可扩展性 | Prompt、JSON Schema、API Provider 可替换 |
| NF-03 | 成本控制 | 开发阶段 Token 用量可估算；避免无限循环请求 |
| NF-04 | Unity 版本 | 建议 Unity 2022 LTS 或与现有经验一致的版本 |

### 2.3 LLM 结构化输出 Schema（第一版）

```json
{
  "reply": "字符串，NPC 对玩家说的话",
  "affectionDelta": 0,
  "triggerQuest": ""
}
```

**字段说明：**

| 字段 | 类型 | 说明 |
|------|------|------|
| `reply` | string | 展示给玩家的 NPC 台词 |
| `affectionDelta` | int | 好感度变化量，可为负 |
| `triggerQuest` | string | 空字符串表示不触发；非空为任务 ID（如 `quest_forest_02`） |

---

## 三、技术架构

### 3.1 分层结构

```
┌─────────────────────────────────────────┐
│  Presentation（UI）                      │
│  DialoguePanel, AffectionView           │
└─────────────────┬───────────────────────┘
                  │
┌─────────────────▼───────────────────────┐
│  Application（NpcBrain）                 │
│  组装 Prompt、维护历史、调用解析           │
└───────┬─────────────────┬───────────────┘
        │                 │
┌───────▼──────┐   ┌──────▼──────────────┐
│ LlmApiClient │   │ GameStateService     │
│ HTTP 请求    │   │ 好感度、任务、NPC状态  │
└──────────────┘   └──────┬──────────────┘
                          │
                   ┌──────▼──────────────┐
                   │ NpcStateMachine      │
                   │ Idle/Busy/Combat/... │
                   └─────────────────────┘
```

### 3.2 数据流

```
玩家输入
  → NpcBrain.BuildPrompt（人设 + GameState + 历史）
  → LlmApiClient.SendChatCompletion
  → LlmResponseParser.ParseJson
  → GameStateService.Apply（好感度、任务）
  → NpcStateMachine.CanTalk（是否允许）
  → DialoguePanel.ShowReply
```

### 3.3 建议目录与类结构

```
Assets/
├── Scenes/
│   └── DemoScene.unity
├── Scripts/
│   ├── Configuration/
│   │   ├── LlmConfiguration.cs          # API 地址、模型名（Key 从外部读取）
│   │   └── NpcPersonaConfiguration.cs   # 人设 ScriptableObject
│   ├── Llm/
│   │   ├── LlmApiClient.cs
│   │   ├── ChatMessage.cs
│   │   ├── LlmChatRequest.cs
│   │   ├── LlmChatResponse.cs
│   │   └── LlmResponseParser.cs
│   ├── Game/
│   │   ├── GameStateService.cs
│   │   ├── QuestDefinition.cs
│   │   └── AffectionChangedEvent.cs
│   ├── Npc/
│   │   ├── NpcController.cs
│   │   ├── NpcBrain.cs
│   │   └── NpcStateMachine.cs
│   └── UI/
│       ├── DialoguePanel.cs
│       └── PlayerMovementController.cs
├── ScriptableObjects/
│   └── Personas/
│       └── ForestNpcPersona.asset
└── StreamingAssets/  （可选，不放 Key）
```

**根目录：**

```
.gitignore          # 必须忽略 LlmSecrets.json、.env
LlmSecrets.json.example
README.md
Docs/
└── Unity-Llm-Npc-Demo-任务清单.md   # 本文档
```

---

## 四、任务清单（按周）

> 勾选框用于自行跟踪进度：`[ ]` 未完成 · `[x]` 已完成

### Phase 0：准备（第 0 周，约 4 小时）

- [ ] **P0-01** 创建独立 GitHub 仓库（建议名：`Unity-Llm-Npc-Demo`）
- [ ] **P0-02** 选择 LLM 服务商并注册 API Key（DeepSeek / 智谱 / 通义 等）
- [ ] **P0-03** 创建 Unity 工程（2022 LTS），初始化 `.gitignore`
- [ ] **P0-04** 添加 `LlmSecrets.json.example`，真实 Key 放本地 ignored 文件
- [ ] **P0-05** 阅读所选 API 的 Chat Completions 文档与 JSON 输出说明

**理论（约 2h）：** OpenAI 兼容 API 的消息结构（system / user / assistant）

---

### Phase 1：Week 1 — 跑通 API + 最小场景（约 20h）

**项目（约 14h）**

- [ ] **W1-01** 搭建 Demo 场景：平面 / Tilemap、玩家占位、NPC 占位
- [ ] **W1-02** 实现 `PlayerMovementController`（基础移动）
- [ ] **W1-03** 实现 `DialoguePanel`：输入框、发送、文本展示
- [ ] **W1-04** 实现 `LlmConfiguration` + 从本地 JSON 读取 API Key
- [ ] **W1-05** 实现 `LlmApiClient`：单次非流式 Chat Completion
- [ ] **W1-06** 实现 `ChatMessage` 与请求体序列化（`JsonUtility` 或 `Newtonsoft.Json`）
- [ ] **W1-07** 对话按钮 → 调用 API → 显示纯文本回复（暂不解析 JSON）
- [ ] **W1-08** 提交 Week 1 代码，README 写「环境配置」章节

**理论（约 6h）**

- [ ] **W1-T01** 理解 HTTP POST + Authorization Header
- [ ] **W1-T02** 笔记：所选 API 的 base URL、model 名称、限流规则
- [ ] **W1-T03** 笔记：Unity 中 `UnityWebRequest` vs `HttpClient` 选型（选一个并记录原因）

**Week 1 完成标准：** 在 Unity 内输入一句话，能收到 LLM 文本回复。

---

### Phase 1：Week 2 — 人设 + 多轮记忆（约 20h）

**项目（约 14h）**

- [ ] **W2-01** 创建 `NpcPersonaConfiguration`（ScriptableObject）：名称、性格、背景、禁忌
- [ ] **W2-02** 实现 `NpcBrain`：组装 System Prompt 模板
- [ ] **W2-03** 维护 `List<ChatMessage>` 对话历史
- [ ] **W2-04** 实现滑动窗口：超过 N 轮时丢弃最早 user/assistant 对
- [ ] **W2-05** 在 System Prompt 中要求 **仅输出 JSON**（附 Schema 示例）
- [ ] **W2-06** 实现 `LlmResponseParser`：从回复中提取 JSON（处理 markdown 代码块包裹）
- [ ] **W2-07** 定义 `NpcDialogueResult` 数据类（reply / affectionDelta / triggerQuest）
- [ ] **W2-08** UI 只展示 `reply` 字段

**理论（约 6h）**

- [ ] **W2-T01** 阅读 [Prompt Engineering Guide](https://www.promptingguide.ai/zh) 的 Role / Few-shot 章节
- [ ] **W2-T02** 练习：写 3 版 System Prompt，对比 JSON 输出稳定性
- [ ] **W2-T03** 笔记：常见 JSON 失败形态（多余文字、单引号、 trailing comma）

**Week 2 完成标准：** 多轮对话保持人设；回复可稳定解析为 JSON 对象。

---

### Phase 1：Week 3 — GameState 联动（约 20h）

**项目（约 14h）**

- [ ] **W3-01** 实现 `GameStateService`：好感度（int）、当前任务 ID（string）
- [ ] **W3-02** 定义至少 2 个 `QuestDefinition`（ID、标题、描述、完成条件简述）
- [ ] **W3-03** Prompt 注入：`当前好感度：X；进行中任务：Y`
- [ ] **W3-04** 解析 JSON 后调用 `GameStateService.ApplyAffectionDelta`
- [ ] **W3-05** 解析 JSON 后调用 `GameStateService.TryTriggerQuest`
- [ ] **W3-06** UI 显示好感度变化（数字或简单进度条）
- [ ] **W3-07** 任务触发时在 UI 或 Console 给出明确反馈
- [ ] **W3-08** 实现请求中 Loading 状态，禁止连点发送

**理论（约 6h）**

- [ ] **W3-T01** 笔记：对话记忆策略（滑动窗口 vs 摘要压缩）
- [ ] **W3-T02** 估算一次请求的 Token 量（System + 历史 + 用户输入）
- [ ] **W3-T03** 笔记：游戏状态注入 Prompt 的利弊（新鲜度 vs Token 成本）

**Week 3 完成标准：** 对话能改变好感度并触发至少 1 个任务。

---

### Phase 1：Week 4 — FSM 约束 + 健壮性 + 交付（约 20h）

**项目（约 14h）**

- [ ] **W4-01** 实现 `NpcStateMachine`：Idle / Busy / Combat / Talking
- [ ] **W4-02** 对话前检查 `CanStartDialogue()`；Combat/Busy 返回 fallback 台词
- [ ] **W4-03** 对话开始 → Talking；结束 → 回到 Idle
- [ ] **W4-04** 提供 Debug 方式切换 NPC 状态（按键或 Inspector 按钮）
- [ ] **W4-05** JSON 解析失败：重试 1 次（附带「上次格式错误」提示）
- [ ] **W4-06** 仍失败：使用固定 fallback 台词，不崩溃
- [ ] **W4-07** 网络超时（如 30s）提示用户
- [ ] **W4-08** 完善 README：架构图、配置步骤、功能列表
- [ ] **W4-09** 录制 3～5 分钟演示视频并上传到 B 站 / 链接写入 README
- [ ] **W4-10** 打 Tag `v0.1-b-tier`，作为 Phase 1 里程碑

**理论（约 6h）**

- [ ] **W4-T01** 笔记：FSM 与 LLM 的职责边界（谁决定「能不能聊」vs 「聊什么」）
- [ ] **W4-T02** 整理 5 条面试问答（见本文档 §七）
- [ ] **W4-T03** 复盘：Prompt 最容易出问题的 3 个 case + 修复方式

**Week 4 完成标准：** B 档 Demo 可演示、可复现、可讲清楚架构。

---

### Phase 2：Week 5～6 — 打磨 + 传统 AI + 求职（约 40h）

**项目（约 28h）**

- [ ] **W5-01** 为 NPC 添加 Idle 巡逻（简单路点或随机走动）
- [ ] **W5-02** Combat 状态下 NPC 追击玩家或播放攻击动画（简化版即可）
- [ ] **W5-03** 优化 Prompt 与 Persona，减少 OOC（Out of Character）
- [ ] **W5-04** 优化 UI 布局与交互反馈（可选：打字机效果）
- [ ] **W5-05** 代码整理：命名空间、删除调试日志、补充关键注释
- [ ] **W6-01** 更新个人简历：新增本项目条目 + 专业技能一行
- [ ] **W6-02** 准备项目介绍稿（1 分钟 / 3 分钟两个版本）
- [ ] **W6-03** 开始投递：游戏客户端岗为主，AI 相关 JD 为备

**理论（约 12h）**

- [ ] **W5-T01** 阅读《AI for Games》行为树章节（2h）
- [ ] **W5-T02** 笔记：行为树 vs FSM 适用场景（2h）
- [ ] **W6-T01** 整理 10 道客户端 + AI 集成面试题自答（4h）
- [ ] **W6-T02** 看 1 个 GDC NPC / AI 相关演讲（2h）
- [ ] **W6-T03** 了解目标公司是否在招「智能化 / NLP / 大模型」相关客户端（2h）

**Phase 2 完成标准：** 简历可投；面试能流畅介绍 Demo。

---

### Phase 3：Week 7～10 — Python 网关 + RAG（可选，冲备 B 岗）

**项目（约 28h）**

- [ ] **W7-01** 搭建 Python FastAPI 项目：`POST /chat`
- [ ] **W7-02** API Key 移至服务端；Unity 只带 session / player id
- [ ] **W7-03** 请求/响应日志 + 简单限流
- [ ] **W8-01** 准备世界观 / 角色设定 Markdown 文档
- [ ] **W8-02** 实现 RAG：文档切块 + 向量检索（可用 Chroma / FAISS 等）
- [ ] **W8-03** 检索结果注入 Prompt，对比有无 RAG 的 lore 准确性
- [ ] **W9-01** Unity 改为调用 FastAPI 而非直连厂商 API
- [ ] **W9-02** README 增加「后端部署」章节
- [ ] **W10-01** 录屏对比版：展示 RAG 防胡编案例
- [ ] **W10-02** Tag `v0.2-rag-gateway`

**理论（约 12h）**

- [ ] **W7-T01** RAG 原理：嵌入、检索、重排序（概念级）
- [ ] **W8-T01** Python FastAPI 基础（若尚未熟悉）
- [ ] **W9-T01** 笔记：网关层价值（安全、日志、切换模型、缓存）

---

### Phase 4：Week 11～12 — 扩展方向（二选一）

**选项 A：GameJam AI 小作品**

- [ ] **W11-01** 选定 GameJam 主题
- [ ] **W11-02** 48～72h 内做「AI 驱动玩法」最小可玩版本
- [ ] **W12-01** Jam 作品 README + 录屏

**选项 B：HimiiEngine 集成试验**

- [ ] **W11-01** 在 HimiiEngine 中复用 `LlmApiClient` 思路（C# 层）
- [ ] **W11-02** 最小场景：ImGui 对话窗口 + NPC 实体
- [ ] **W12-01** 写技术博客：Unity Demo vs 自研引擎集成差异

---

## 五、理论学习计划（按里程碑，约 6h/周）

| 周次 | 主题 | 产出 |
|------|------|------|
| 1 | API 消息结构、Unity HTTP 调用 | 笔记 1 页 |
| 2 | Prompt 设计、JSON 约束 | 3 版 Prompt 对比记录 |
| 3 | 对话记忆、Token 估算 | 记忆策略笔记 |
| 4 | FSM vs LLM 分工、健壮性 | 面试 Q&A 5 条 |
| 5 | 行为树 / 传统游戏 AI 基础 | 笔记 + Demo 巡逻功能 |
| 6 | 面试整理、行业 JD 调研 | 10 道自答 + 投递记录 |
| 7+ | RAG / FastAPI（若进入 Phase 3） | 后端 README |

**延后学习（暂不安排）：**

- Python 系统课、线代/概率、神经网络训练
- Unity ML-Agents、强化学习
- 本地大模型部署与量化

---

## 六、推荐资源

### LLM 工程

- [Prompt Engineering Guide（中文）](https://www.promptingguide.ai/zh)
- 所选 API 官方文档：Chat Completions、JSON Mode / 结构化输出

### 传统游戏 AI

- 《Artificial Intelligence for Games》（Millington）— 行为树、FSM 章节
- GDC：Behavior Trees 相关演讲（建立直觉即可）

### 工程实践

- Unity `UnityWebRequest` 文档
- `.gitignore` 模板：忽略 `LlmSecrets.json`、`.env`、`Library/`

---

## 七、面试话术备忘

### 7.1 一分钟项目介绍（模板）

> 这是一个 Unity 客户端 Demo，把国内大模型 API 集成进 NPC 对话系统。我用 System Prompt 约束人设，把好感度和任务状态注入每次请求，并要求模型返回 JSON，由客户端解析后驱动任务触发。同时用 FSM 管「能不能聊」——战斗或忙碌状态下不会走 LLM，避免逻辑冲突。API Key 不进仓库，JSON 解析失败有重试和兜底。

### 7.2 高频问题

| 问题 | 回答要点 |
|------|----------|
| 和 ChatGPT 套壳有什么区别？ | 游戏状态双向联动、结构化输出驱动玩法、FSM 约束、记忆与成本控制 |
| 如何防止 LLM 胡编设定？ | 第一版靠 Prompt + 状态注入；第二版计划 RAG 检索设定文档 |
| JSON 解析失败怎么办？ | 重试 + schema 校验 + fallback 台词，保证游戏不崩 |
| Key 怎么管理？ | 本地 secrets + gitignore；生产应走服务端网关 |
| 为什么不用行为树做全部对话？ | 固定对话缺变化；LLM 管内容，FSM 管规则，职责分离 |

### 7.3 简历条目（Demo 完成后粘贴）

**专业技能（追加一行）：**

> 熟悉 LLM 在游戏中的应用：Prompt 工程、多轮对话记忆、结构化输出与游戏状态机联动；有 Unity 集成国内大模型 API 的实战 Demo。

**项目经历（新增）：**

> **LLM 驱动 NPC 对话系统（Unity）** · 个人项目 · 2026  
> 实现 Prompt + 游戏状态（好感度/任务）双通道约束；LLM 结构化 JSON 驱动任务触发；FSM 限制 NPC 行为状态；API Key 环境隔离与解析失败兜底。  
> 技术栈：Unity / C# / 国内大模型 API / FSM

---

## 八、风险与缓解

| 风险 | 影响 | 缓解措施 |
|------|------|----------|
| Scope 膨胀 | 无法按时交付 | 严格 B 档；Backlog 统一管理 |
| API Key 泄露 | 安全 / 费用 | gitignore + example 文件 + README 说明 |
| JSON 不稳定 | 游戏逻辑中断 | 重试 + fallback + Prompt 强化 |
| Token 成本超预期 | 开发预算 | 滑动窗口 + 限制调试次数 |
| 面试被问 ML 原理 | 备 B 岗深度不足 | 诚实边界 + Phase 3 RAG 补工程深度 |
| 与 HYProject 时间冲突 | 每周 20h 完不成 | 优先 Week 1～4 里程碑，其余弹性 |

---

## 九、进度总览看板

| Phase | 周次 | 里程碑 | 状态 |
|-------|------|--------|------|
| 0 | 准备 | 仓库 + API Key + Unity 工程 | ⬜ 未开始 |
| 1 | W1 | API 跑通 | ⬜ 未开始 |
| 1 | W2 | 人设 + 记忆 + JSON | ⬜ 未开始 |
| 1 | W3 | GameState 联动 | ⬜ 未开始 |
| 1 | W4 | FSM + 交付 v0.1 | ⬜ 未开始 |
| 2 | W5～6 | 打磨 + 简历投递 | ⬜ 未开始 |
| 3 | W7～10 | Python + RAG v0.2 | ⬜ 可选 |
| 4 | W11～12 | GameJam / HimiiEngine | ⬜ 可选 |

---

## 十、本周立即行动（Week 1 前 3 项）

1. [ ] 创建 GitHub 仓库并 clone 到本地  
2. [ ] 注册 API，写通第一个 C# 控制台或 Unity 脚本请求  
3. [ ] 在 Unity 场景中放 1 个 NPC + 1 个输入框，能显示回复  

---

*文档随项目进展更新。Phase 1 完成后建议在本文件顶部更新「最后更新」日期，并勾选已完成任务。*
