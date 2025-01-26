; leftLength = rightLength
; ecx: length
; rdx: left
; r8: right
; r9: result
; ret: size

.code

AddByEqualLength proc export
	push rcx

do_while:
	mov rax, [rdx]
	mov rbx, [r8]
	adc rax, rbx
	mov [r9], rax

	lea rdx, [rdx + 8]
	lea r8, [r8 + 8]
	lea r9, [r9 + 8]

	loopnz do_while
;do_while_end

	jnc noCarry
;carry:
	mov rax, 1
	mov [r9], rax
	pop rax
	inc rax
	ret
noCarry:
	pop rax
	ret
AddByEqualLength endp

end