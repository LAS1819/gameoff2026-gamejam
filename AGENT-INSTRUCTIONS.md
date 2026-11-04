# Agent Instructions for Jamgent

## ⛔ JAMGENT MUST NEVER COMMIT

**The Jamgent AI agent is FORBIDDEN from running `git commit`.**

This is a behavioral rule, not a technical block. The user controls all commits.

### Why?

- Commits are permanent records and should only be made by the human user
- The agent's role is to prepare code changes, not finalize them
- This ensures accountability and user control over repository history
- Only the user can decide when changes are ready and write meaningful commit messages

---

## 📋 Correct Workflow

### For Jamgent (the Agent):

1. **Read** and understand the user's request
2. **Edit** or **create** files as needed using `view`, `create`, `edit` tools
3. **Prepare** changes (they will be staged or visible in `git status`)
4. **Explain** what was changed and why
5. ⛔ **STOP** — Never run `git commit`

### For the User:

1. Review the agent's changes with `git diff` or `git status`
2. Run `git commit -m "your message"` yourself when satisfied
3. Run `git push` when ready
4. **Full control stays with you**

---

## 🚫 Forbidden Commands (for Agent)

The following are **NEVER** executed by Jamgent:

```bash
git commit
git push
git revert
git reset
git cherry-pick
```

These require user approval and manual execution.

---

## ✅ Allowed Commands (for Agent)

Jamgent CAN run these commands:

```bash
git status              # Check current state
git diff               # Review changes
git log                # Check history
git branch             # Check branches
git add                # Stage changes (if instructed)
```

---

## 📝 Summary

| Action | Who | When |
|--------|-----|------|
| Edit/Create/Delete files | Agent | During task |
| Review changes with git diff | Agent or User | Anytime |
| Review changes in UI | User | Before commit |
| Run `git commit` | **User Only** | When satisfied |
| Run `git push` | **User Only** | Final step |

**The agent prepares. The user decides. The user commits.**
