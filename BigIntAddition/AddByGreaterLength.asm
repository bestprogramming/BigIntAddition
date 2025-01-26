; leftLength > rightLength
; ecx: leftLength
; edx: rightLength
; r8: left
; r9: right
; rdi: result
; ret: size

.code

AddByGreaterLength proc export
	mov rdi, [rsp + 40]
	push rcx
	sub ecx, edx

do_while_right:
	mov rax, [r8]
	mov rbx, [r9]
	adc rax, rbx
	mov [rdi], rax

	lea r8, [r8 + 8]
	lea r9, [r9 + 8]
	lea rdi, [rdi + 8]

	dec edx
	jnz do_while_right
;do_while_right_end

	jnc while_carry_end

while_carry:
	mov rax, [r8]
	add rax, 1
	mov [rdi], rax
	
	lea r8, [r8 + 8]
	lea rdi, [rdi + 8]

	dec ecx
	jc while_carry
while_carry_end:
	
	cmp ecx, 0
	jz noCarry
	jle carryToEnd

	mov rsi, r8
	rep movsq
	pop rax
	ret

noCarry:
	pop rax
	ret
carryToEnd:
	mov rax, 1
	mov [rdi - 8], rax
	pop rax
	inc rax
	ret
AddByGreaterLength endp

end